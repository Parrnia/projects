import { useState } from 'react';
import Navbar from './Navbar';
import UserWallet from './wallet';

const Navbarparent = () => {
  const [openWallet, setOpenWallet] = useState(false);

  return (
    <div>
      <div>
    <Navbar openWallet={openWallet} setOpenWallet={setOpenWallet} /></div>
      <UserWallet open={openWallet} setOpen={setOpenWallet} />
    </div>
  );
};
export default Navbarparent;