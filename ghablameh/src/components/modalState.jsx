import { createGlobalState } from 'react-hooks-global-state';

const initialState = {
  showMyModel: false,
};

export const useModalState = () => {
  const { useGlobalState } = createGlobalState(initialState);
  const showMyModel = useGlobalState('showMyModel');
  const setShowMyModel = useGlobalState('showMyModel', 'setShowMyModel');

  return { showMyModel, setShowMyModel };
};